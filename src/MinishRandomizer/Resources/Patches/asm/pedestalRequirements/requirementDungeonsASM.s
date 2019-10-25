.thumb
push	{r4-r5,lr}
@get target, make sure it is in range
ldr	r4,dungeons
mov	r5,#0	@counter
cmp	r4,#0
bne	min
mov	r4,#1
min:
cmp	r4,#6
bls	max
mov	r4,#6
max:

@check if we completed dws
ldr	r0,=#0x2002C9C
ldrb	r0,[r0]
mov	r1,#0x04
and	r0,r1
cmp	r0,#0
beq	next1
add	r5,#1

@check if we completed cof
next1:
ldr	r0,=#0x2002C9C
ldrb	r0,[r0]
mov	r1,#0x08
and	r0,r1
cmp	r0,#0
beq	next2
add	r5,#1

@check if we completed fow
next2:
ldr	r0,=#0x2002D72
ldrb	r0,[r0]
mov	r1,#0x02
and	r0,r1
cmp	r0,#0
beq	next3
add	r5,#1

@check if we completed tod
next3:
ldr	r0,=#0x2002C9C
ldrb	r0,[r0]
mov	r1,#0x20
and	r0,r1
cmp	r0,#0
beq	next4
add	r5,#1

@check if we completed rc
next4:
ldr	r0,=#0x2002D02
ldrb	r0,[r0]
mov	r1,#0x04
and	r0,r1
cmp	r0,#0
beq	next5
add	r5,#1

@check if we completed pow
next5:
ldr	r0,=#0x2002C9C
ldrb	r0,[r0]
mov	r1,#0x40
and	r0,r1
cmp	r0,#0
beq	next6
add	r5,#1

@check result
next6:
cmp	r4,r5
bhi	false

true:
mov	r0,#1
pop	{r4-r5,pc}

false:
mov	r0,#0
pop	{r4-r5,pc}
.align
.ltorg
dungeons:
