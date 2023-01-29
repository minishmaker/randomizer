.thumb
push	{lr}
@check if we are in the menu
ldr	r0,=#0x3001004
ldrb	r0,[r0]
cmp	r0,#7
beq	nobombcheck

@check if there are bombs in the room
mov	r0,#8
mov	r1,#2
mov	r2,#2
ldr	r3,=#0x805E588
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	doneBomb
nobombcheck:

@check if remote bombs are equipped
ldr	r0,=#0x2002AF4
ldrb	r1,[r0]
cmp	r1,#8
beq	remote
ldrb	r2,[r0,#1]
cmp	r2,#8
beq	remote

@check if bombs are equipped
cmp	r1,#7
beq	noRemote
cmp	r2,#7
beq	noRemote

@check if remote bombs unlocked and not off
mov	r0,#0x08
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
mov	r1,#2
and	r1,r0
cmp	r1,#0
bne	noRemote
cmp	r0,#0
beq	noRemote
remote:
mov	r0,#0x18
mov	r1,r0
ldr	r3,=#0x801D824
mov	lr,r3
.short	0xF800
b	doneBomb
noRemote:
mov	r0,#0x19
mov	r1,r0
ldr	r3,=#0x801D824
mov	lr,r3
.short	0xF800

doneBomb:
@check if light bow is unlocked and not off
mov	r0,#0x0A
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
mov	r1,#2
and	r1,r0
cmp	r1,#0
bne	noLight
cmp	r0,#0
beq	noLight
mov	r0,#0x1D
mov	r1,r0
ldr	r3,=#0x801D824
mov	lr,r3
.short	0xF800
b	doneBow
noLight:

doneBow:
@check if magic boomerang unlocked and not off
mov	r0,#0x0C
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
mov	r1,#2
and	r1,r0
cmp	r1,#0
bne	noMagic
cmp	r0,#0
beq	noMagic
mov	r0,#0x1C
mov	r1,r0
ldr	r3,=#0x801D824
mov	lr,r3
.short	0xF800
b	doneBoomerang
noMagic:
mov	r0,#0x1B
mov	r1,r0
ldr	r3,=#0x801D824
mov	lr,r3
.short	0xF800

doneBoomerang:
@fix the bomb flags, check if we own both type of bombs
ldr	r0,=#0x2002B33
ldrb	r1,[r0]
mov	r2,#0xC0
and	r2,r1
cmp	r2,#0
beq	end
ldrb	r1,[r0,#1]
mov	r2,#0x03
and	r2,r1
cmp	r2,#0
beq	end

@check if we have bombs equipped
ldr	r0,=#0x2002AF4
ldrb	r1,[r0]
cmp	r1,#7
beq	setbomb
ldrb	r2,[r0,#1]
cmp	r2,#7
beq	setbomb

@check if we have remote bombs equipped
ldr	r0,=#0x2002AF4
ldrb	r1,[r0]
cmp	r1,#8
beq	setremote
ldrb	r2,[r0,#1]
cmp	r2,#8
beq	setremote

end:
pop	{pc}

setbomb:
ldr	r0,=#0x2002B33
ldrb	r1,[r0]
mov	r2,#0x7F
and	r1,r2
mov	r2,#0x40
orr	r1,r2
strb	r1,[r0]
ldrb	r1,[r0,#1]
mov	r2,#0xFE
and	r1,r2
mov	r2,#0x02
orr	r1,r2
strb	r1,[r0,#1]
b	end

setremote:
ldr	r0,=#0x2002B33
ldrb	r1,[r0]
mov	r2,#0xBF
and	r1,r2
mov	r2,#0x80
orr	r1,r2
strb	r1,[r0]
ldrb	r1,[r0,#1]
mov	r2,#0xFD
and	r1,r2
mov	r2,#0x01
orr	r1,r2
strb	r1,[r0,#1]
b	end
