.thumb
push	{lr}
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
pop	{pc}
