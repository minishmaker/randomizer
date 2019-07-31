.thumb
@vanilla check
add	r1,r0
ldrb	r0,[r1,#3]
cmp	r0,#0
beq	noCutscene

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
bne	noCutscene

cutscene:
ldr	r3,=#0x80A35ED
bx	r3

noCutscene:
ldr	r3,=#0x80A3601
bx	r3

.align
.ltorg
always:
