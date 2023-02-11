.equ showMap, always+4
.thumb
@vanilla check
add	r1,r0
ldrb	r0,[r1,#3]
cmp	r0,#0
@vanilla behavior for gold fusions
beq	return

@check if we always skip
ldr	r3,always
cmp	r3,#0
bne	noCutscene

@check if a skip button is being held
ldr	r3,=#0x3000FF0
ldrh	r3,[r3]
mov	r2,#0x0B
and	r3,r2
cmp	r3,#0
beq	cutscene

noCutscene:
ldr	r3,showMap
cmp	r3,#0
beq	fullSkip

@check if fusion has a map marker (Biggoron)
ldrb	r3,[r1,#7]
cmp	r3,#0
beq	fullSkip

@if its a beanstalk, we need to run extra code
mov	r2,#0
cmp	r3,#6
beq	fullSkip

skipToMap:
@go straight to map screen
mov	r0,#0x0A

cutscene:
ldr	r3,=#0x80A35ED
bx	r3

fullSkip:
push {r1,r2,r4}
@get the fusion id into place
ldrb	r0,[r1,#3]
ldrb	r1,[r1,#4]
ldr	r2,=#0x2032EC0
strb	r0,[r2,#2]
strb	r1,[r2,#3]
strb	r0,[r2,#4]
strb	r1,[r2,#5]
@prepare the fusion cutscene
ldr	r4,=#0x2032EC0
ldrb	r1,[r4,#3]
lsl	r0,r1,#2
add	r0,r1
lsl	r0,#2
ldr	r1,=#0x8054460
ldr	r1,[r1]
add	r0,r1
ldr	r2,=#0x2000080
ldrb	r1,[r0]
mov	r3,#0
strb	r1,[r2]
ldrb	r1,[r0,#1]
strb	r1,[r2,#3]
ldrb	r1,[r4,#3]
strb	r1,[r2,#4]
str	r0,[r2,#0xC]
ldrb	r0,[r2,#5]
add	r0,#1
strb	r0,[r2,#5]
strb	r3,[r2,#6]
mov	r0,#0x96
lsl	r0,#1
strh	r0,[r2,#8]
pop	{r1,r2,r4}

@check for beanstalk map marker
ldrb	r3,[r1,#7]
cmp	r3,#6
bne	return

beanstalkFlag:
@beanstalks need an extra flag set
@first part of WorldEvent_Beanstalk_0
push	{r2,r4-r5}
ldr	r5, =#0x2000080
ldr	r4, [r5, #0x0C]
ldr	r1, =#0x8055180
ldr	r1, [r1]
ldrb	r0, [r4, #0x11]
lsl	r0, #1
add	r0, r1
ldrh	r0, [r0]
ldrh	r1, [r4, #0x12]
ldr	r3, =#0x807C6C0
mov	lr, r3
.short	0xF800
pop	{r2,r4-r5}

@check type of skip
cmp	r2,#0
beq	skipToMap

return
ldr	r3,=#0x80A3601
bx	r3

.align
.ltorg
always:
