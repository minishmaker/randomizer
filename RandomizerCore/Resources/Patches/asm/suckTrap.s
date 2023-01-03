.equ getRNG, spawnEnemy+4
.thumb
push	{lr}
ldr	r0,getRNG
mov	lr,r0
.short	0xF800
mov	r1,#3
and	r0,r1
cmp	r0,#0
beq	normal
cmp	r0,#1
beq	green
cmp	r0,#2
beq	blue
b	red
normal:
mov	r1,#0x14
mov	r2,#0
b	spawn
green:
mov	r1,#0x17
mov	r2,#0
b	spawn
blue:
mov	r1,#0x17
mov	r2,#1
b	spawn
red:
mov	r1,#0x17
mov	r2,#2
b	spawn
spawn:
ldr	r0,=#0x3001160
ldr	r3,spawnEnemy
mov	lr,r3
.short	0xF800
pop	{pc}
.align
.ltorg
spawnEnemy:
